import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import * as bootstrap from 'bootstrap';
import { AfterViewInit } from '@angular/core';

@Component({
  selector: 'app-medico',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './medico.component.html',
})
export class MedicoComponent implements OnInit, AfterViewInit {
  citas: any[] = [];
  mensaje: string = '';
  editarEstadoForm!: FormGroup;
  citaSeleccionadaId: number | null = null;
  modalEditarEstado: any = null;


  constructor(
    private http: HttpClient,
    private auth: AuthService,
    private fb: FormBuilder
  ) {}

  ngAfterViewInit(): void {
    const modalElement = document.getElementById('modalEditarEstado');
    if (modalElement) {
      this.modalEditarEstado = new bootstrap.Modal(modalElement);
    }
  }

  ngOnInit(): void {
    this.editarEstadoForm = this.fb.group({
      estado: ['Pendiente'],
    });

    const idUsuario = this.auth.obtenerIdUsuarioDesdeToken();
    if (idUsuario) {
      this.obtenerCitasMedico(idUsuario);
    } else {
      this.mensaje = 'No se pudo obtener el ID del médico.';
    }
  }

  obtenerCitasMedico(id: number): void {
    this.http
      .get<any[]>(`http://localhost:5147/api/Citas/medico/${id}`)
      .subscribe({
        next: (data) => (this.citas = data),
        error: (err) => {
          console.error('Error al cargar citas del médico:', err);
          this.mensaje = 'Error al cargar citas.';
        },
      });
  }

  seleccionarCita(id: number, estadoActual: string): void {
    this.citaSeleccionadaId = id;
    this.editarEstadoForm.patchValue({ estado: estadoActual });
    if (this.modalEditarEstado) {
      this.modalEditarEstado.show();
    }
  }

  actualizarEstado(): void {
    if (!this.citaSeleccionadaId) return;

    const nuevoEstado = this.editarEstadoForm.value.estado;

    this.http
      .put(
        `http://localhost:5147/api/Citas/${this.citaSeleccionadaId}/estado`,
        {
          estado: nuevoEstado,
        }
      )
      .subscribe({
        next: () => {
          this.mensaje = 'Estado actualizado correctamente';
          const idUsuario = this.auth.obtenerIdUsuarioDesdeToken();
          if (idUsuario) this.obtenerCitasMedico(idUsuario);
          console.log(this.citaSeleccionadaId);

          if (this.modalEditarEstado) {
            this.modalEditarEstado.hide();
          }
        },
        error: (err) => {
          console.error('Error al actualizar estado:', err);
          this.mensaje = 'Error al actualizar el estado.';
        },
      });
  }
}

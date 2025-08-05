import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';


@Component({
  selector: 'app-citas',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './citas.component.html',
  providers: [AuthService],
})
export class CitasComponent {
  citaForm!: FormGroup;
  mensaje = '';
  consultorios: any[] = [];

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.citaForm = this.fb.group({
      fecha: ['', Validators.required],
      hora: ['', Validators.required],
      estado: ['Pendiente'],
      observaciones: [''],
      idConsultorio: ['', Validators.required],
    });

    this.obtenerConsultorios();
  }

  obtenerConsultorios() {
    this.http.get<any[]>('http://localhost:5147/api/Consultorios').subscribe({
      next: (data) => (this.consultorios = data),
      error: (err) => console.error(err),
    });
  }

  crearCita() {
    const usuarioId = this.obtenerUsuarioId();

    if (!usuarioId) {
      this.mensaje = 'No se pudo obtener el usuario autenticado.';
      this.router.navigate(['/login']);
      return;
    }

    const cita = {
      fecha: this.citaForm.value.fecha,
      hora: this.citaForm.value.hora,
      estado: 'Pendiente',
      observaciones: this.citaForm.value.observaciones,
      idUsuario: usuarioId,
      idConsultorio: this.citaForm.value.idConsultorio,
    };

    this.http.post('http://localhost:5147/api/Citas', cita).subscribe({
      next: () => {
        this.mensaje = 'Cita creada exitosamente';
        this.router.navigate(['/dashboard']);
      },
      error: () => (this.mensaje = 'Error al crear la cita'),
    });
  }

  obtenerUsuarioId(): number | null {
    return this.authService.obtenerIdUsuarioDesdeToken();
  }
}

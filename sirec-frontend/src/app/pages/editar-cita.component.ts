import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-editar-cita',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './editar-cita.component.html',
})
export class EditarCitaComponent implements OnInit {
  citaForm!: FormGroup;
  citaId!: number;
  mensaje = '';
  consultorios: any[] = [];

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.citaId = Number(this.route.snapshot.paramMap.get('id'));

    this.citaForm = this.fb.group({
      fecha: ['', Validators.required],
      hora: ['', Validators.required],
      observaciones: [''],
      estado: ['', Validators.required],
      idConsultorio: ['', Validators.required],
    });

    this.obtenerConsultoriosYLuegoCargarCita();
  }

  obtenerConsultoriosYLuegoCargarCita() {
    this.http.get<any[]>('http://localhost:5147/api/Consultorios').subscribe({
      next: (consultorios) => {
        this.consultorios = consultorios;
        this.obtenerCita(); // solo después de tener los consultorios
      },
      error: (err) => console.error('Error al cargar consultorios:', err),
    });
  }

  obtenerConsultorios() {
    this.http.get<any[]>('http://localhost:5147/api/Consultorios').subscribe({
      next: (data) => (this.consultorios = data),
      error: (err) => console.error('Error al obtener consultorios:', err),
    });
  }

  obtenerCita() {
    this.http
      .get<any>(`http://localhost:5147/api/Citas/${this.citaId}`)
      .subscribe({
        next: (data) => {
          console.log('Cita obtenida:', data);
          this.citaForm.patchValue({
            fecha: data.Fecha.slice(0, 10),
            hora: data.Hora,
            estado: data.Estado,
            observaciones: data.Observaciones,
            idConsultorio: data.IDConsultorio,
          });
        },
        error: (err) => console.error('Error al cargar la cita:', err),
      });
  }

  actualizarCita() {
    const form = this.citaForm.value;

    const citaActualizada = {
      IDCita: this.citaId,
      Fecha: form.fecha,
      Hora: form.hora,
      Estado: form.estado,
      Observaciones: form.observaciones,
      IDUsuario: this.obtenerUsuarioIdDesdeToken(), // asegúrate de tener este método
      IDConsultorio: form.idConsultorio,
    };

    console.log('Payload enviado:', citaActualizada); // Depuración

    this.http
      .put(`http://localhost:5147/api/Citas/${this.citaId}`, citaActualizada)
      .subscribe({
        next: () => {
          this.mensaje = 'Cita actualizada con éxito';
          this.router.navigate(['/dashboard']);
        },
        error: (err) => {
          console.error('Error al actualizar la cita:', err);
          this.mensaje = 'Error al actualizar la cita';
        },
      });
  }
  obtenerUsuarioIdDesdeToken(): number | null {
    const token = localStorage.getItem('token');
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const claim =
        'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';
      return parseInt(payload[claim], 10);
    } catch (e) {
      console.error('Error al decodificar token:', e);
      return null;
    }
  }
}

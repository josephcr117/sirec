import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-solicitar-cita',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './solicitar-cita.component.html',
})
export class SolicitarCitaComponent implements OnInit {
  form!: FormGroup;
  consultorios: any[] = [];
  mensaje: string = '';
  exito: boolean = false;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private auth: AuthService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      idConsultorio: ['', Validators.required],
      fecha: ['', Validators.required],
      hora: ['', Validators.required],
      observaciones: [''],
    });

    this.cargarConsultorios();
  }

  cargarConsultorios(): void {
    this.http.get<any[]>('http://localhost:5147/api/Consultorios').subscribe({
      next: (data) => (this.consultorios = data),
      error: () => console.error('Error al cargar consultorios'),
    });
  }

  solicitarCita(): void {
    const idUsuario = this.auth.obtenerIdUsuarioDesdeToken();
    if (!idUsuario) return;

    const datos = this.form.value;
    const cita = {
      idUsuario,
      idConsultorio: datos.idConsultorio,
      fecha: datos.fecha,
      hora: datos.hora,
      observaciones: datos.observaciones,
      estado: 'Solicitada',
    };

    this.http.post('http://localhost:5147/api/Citas/solicitar', cita).subscribe({
      next: () => {
        this.exito = true;
        this.mensaje = '¡Cita solicitada exitosamente!';
        this.form.reset();
      },
      error: (err) => {
        this.exito = false;
        this.mensaje = 'Error al solicitar cita.';
        console.error(err);
      },
    });
  }
}
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { Router, RouterModule } from '@angular/router';
import feather from 'feather-icons';

@Component({
  selector: 'app-asignar-medico',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './asignar-medico.component.html',
})
export class AsignarMedicoComponent implements OnInit {
  form!: FormGroup;
  medicos: any[] = [];
  consultorios: any[] = [];
  mensaje = '';

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private auth: AuthService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      idUsuario: ['', Validators.required],
      idConsultorio: ['', Validators.required],
    });

    this.obtenerMedicos();
    this.obtenerConsultorios();
  }

  ngAfterViewInit(): void {
    feather.replace();
  }

  obtenerMedicos() {
    this.http
      .get<any[]>('http://localhost:5147/api/Usuario/medicos')
      .subscribe({
        next: (data) => {
          this.medicos = data;
          console.log('Médicos cargados:', this.medicos);
        },
        error: (err) => console.error('Error al cargar médicos', err),
      });
  }

  obtenerConsultorios() {
    this.http.get<any[]>('http://localhost:5147/api/Consultorios').subscribe({
      next: (data) => {
        this.consultorios = data;
        console.log('Consultorios cargados:', this.consultorios);
      },
      error: (err) => console.error('Error al cargar consultorios', err),
    });
  }

  asignar() {
    const datos = this.form.value;

    // Validar que ambos campos sean numéricos válidos
    if (!datos.idUsuario || !datos.idConsultorio) {
      this.mensaje = 'Por favor seleccione un médico y un consultorio.';
      return;
    }

    const payload = {
      idUsuario: Number(datos.idUsuario),
      idConsultorio: Number(datos.idConsultorio),
    };

    this.http
      .post('http://localhost:5147/api/MedicoConsultorio', payload)
      .subscribe({
        next: () => (this.mensaje = 'Asignación exitosa'),
        error: (err) => {
          console.error('Error al asignar:', err);
          this.mensaje = 'Error al asignar médico al consultorio';
        },
      });
  }

  logout(): void {
    this.auth.cerrarSesion();
    this.router.navigate(['/login']);
  }
}

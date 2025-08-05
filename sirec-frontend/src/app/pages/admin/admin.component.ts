import { Component, OnInit, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';

import * as bootstrap from 'bootstrap';
import feather from 'feather-icons';

import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-admin',
  standalone: true,
  templateUrl: './admin.component.html',
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
})
export class AdminComponent implements OnInit, AfterViewInit {
  usuarios: any[] = [];
  usuarioForm!: FormGroup;
  editando: boolean = false;
  usuarioActualId: number | null = null;

  constructor(
    private router: Router,
    private auth: AuthService,
    private http: HttpClient,
    private fb: FormBuilder
  ) {}

  // -------------------------------
  // Ciclo de vida
  // -------------------------------
  ngOnInit(): void {
    this.initForm();
    this.obtenerUsuarios();
  }

  ngAfterViewInit(): void {
    feather.replace();
  }

  // -------------------------------
  // Inicializar formulario
  // -------------------------------
  private initForm(): void {
    this.usuarioForm = this.fb.group({
      nombre: ['', Validators.required],
      apellido: ['', Validators.required],
      correo: ['', [Validators.required, Validators.email]],
      contrasena: ['', Validators.required],
      cedula: ['', Validators.required],
      telefono: ['', Validators.required],
      fechaNacimiento: ['1990-01-01'],
      idRol: ['', Validators.required],
    });
  }

  // -------------------------------
  // Obtener usuarios desde backend
  // -------------------------------
  obtenerUsuarios(): void {
    this.http.get<any[]>('http://localhost:5147/api/Usuario').subscribe({
      next: (data) => {
        this.usuarios = data;
        setTimeout(() => feather.replace(), 0);
      },
      error: (err) => console.error('Error al obtener usuarios:', err),
    });
  }

  // -------------------------------
  // Abrir/Cerrar Modal
  // -------------------------------
  abrirModalAgregar(): void {
    this.editando = false;
    this.usuarioForm.reset();
    this.mostrarModal();
  }

  abrirModalEditar(usuario: any): void {
    this.editando = true;
    this.usuarioActualId = usuario.id;

    this.http
      .get<any>(`http://localhost:5147/api/Usuario/${usuario.id}`)
      .subscribe({
        next: (detalles) => {
          const [nombre, ...resto] = detalles.nombre.split(' ');
          const apellido = resto.join(' ') || '';

          this.usuarioForm.patchValue({
            nombre: nombre,
            apellido: apellido,
            correo: detalles.correo,
            contrasena: '',
            cedula: detalles.cedula,
            telefono: detalles.telefono,
            fechaNacimiento: detalles.fechaNacimiento?.split('T')[0],
            idRol: detalles.usuarioRoles?.[0]?.idRol || 2,
          });

          setTimeout(() => feather.replace(), 0);
          this.mostrarModal();
        },
        error: (err) =>
          console.error('Error al cargar detalles del usuario:', err),
      });
  }

  private mostrarModal(): void {
    const modalElement = document.getElementById('modalAgregarUsuario');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    }
  }

  private cerrarModal(): void {
    const modalElement = document.getElementById('modalAgregarUsuario');
    if (modalElement) {
      const modal = bootstrap.Modal.getInstance(modalElement);
      modal?.hide();
    }
  }

  // -------------------------------
  // Crear o editar usuario
  // -------------------------------
  crearUsuario(): void {
    const datos = this.usuarioForm.value;

    if (this.editando && this.usuarioActualId) {
      const payload = {
        idUsuario: this.usuarioActualId,
        nombre: datos.nombre,
        apellido: datos.apellido,
        correo: datos.correo,
        contraseña: datos.contrasena || 'default',
        cedula: datos.cedula,
        telefono: datos.telefono,
        fechaNacimiento: datos.fechaNacimiento,
        usuarioRoles: [
          {
            idRol: Number(datos.idRol),
            rol: null,
            usuario: null,
          },
        ],
      };

      this.http
        .put(
          `http://localhost:5147/api/Usuario/${this.usuarioActualId}`,
          payload
        )
        .subscribe({
          next: () => {
            this.obtenerUsuarios();
            setTimeout(() => feather.replace(), 0);
            this.cerrarModal();
          },
          error: (err) => console.error('Error al actualizar usuario:', err),
        });
    } else {
      // POST (registro nuevo usuario)
      this.http
        .post(`http://localhost:5147/api/Auth/register?idRol=${datos.idRol}`, {
          nombre: datos.nombre,
          apellido: datos.apellido,
          correo: datos.correo,
          contraseña: datos.contrasena,
          cedula: datos.cedula,
          telefono: datos.telefono,
          fechaNacimiento: datos.fechaNacimiento,
        })
        .subscribe({
          next: () => {
            this.obtenerUsuarios();
            setTimeout(() => feather.replace(), 0);
            this.cerrarModal();
          },
          error: (err) => console.error('Error al registrar usuario:', err),
        });
    }
  }

  // -------------------------------
  // Eliminar usuario
  // -------------------------------
  eliminarUsuario(id: number): void {
    if (confirm('¿Estás seguro de que deseas eliminar este usuario?')) {
      this.http.delete(`http://localhost:5147/api/Usuario/${id}`).subscribe({
        next: () => this.obtenerUsuarios(),
        error: (err) => console.error('Error al eliminar usuario:', err),
      });
    }
  }

  // -------------------------------
  // Cerrar sesión
  // -------------------------------
  logout(): void {
    this.auth.cerrarSesion();
    this.router.navigate(['/login']);
  }
}

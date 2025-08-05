import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
})
export class RegisterComponent {
  registerForm: FormGroup;
  mensaje: string = '';

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      nombre: ['', Validators.required],
      apellido: ['', Validators.required],
      correo: ['', [Validators.required, Validators.email]],
      contraseña: ['', Validators.required],
      cedula: ['', Validators.required],
      telefono: ['', Validators.required],
      fechaNacimiento: ['', Validators.required],
    });
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      this.http
        .post(
          'http://localhost:5147/api/Auth/register',
          this.registerForm.value
        )
        .subscribe({
          next: () => {
            this.mensaje = '¡Registrado con éxito!';
            this.router.navigate(['/login']);
          },
          error: (err) => {
            console.error(err);
            this.mensaje = 'Error al registrar.';
          },
        });
    } else {
      this.mensaje = 'Por favor completa todos los campos.';
    }
  }
}

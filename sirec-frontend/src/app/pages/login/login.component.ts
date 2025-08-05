import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  loginForm: FormGroup;
  mensaje: string = ''; // Mensaje de error

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      correo: ['', [Validators.required, Validators.email]],
      contraseña: ['', Validators.required],
    });
  }

  login() {
    this.auth.login(this.loginForm.value).subscribe({
      next: (res) => {
        this.auth.guardarToken(res.token);
        const rol = this.auth.obtenerRol();

        console.log('ROL:', rol);

        setTimeout(() => {
          if (rol === 'Administrador') {
            this.router.navigate(['/admin']);
          } else if (rol === 'Medico') {
            this.router.navigate(['/medico']);
          } else {
            this.router.navigate(['/dashboard']);
          }
        }, 0);
      },
      error: () => {
        this.mensaje = 'Error al iniciar sesión';
      },
    });
  }

  irRegistro() {
    this.router.navigate(['/register']);
  }
}

import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { CitaService } from '../../services/cita.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  providers: [DatePipe],
})
export class DashboardComponent implements OnInit {
  citas: any[] = [];

  constructor(
    private citaService: CitaService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const idUsuario = this.obtenerUsuarioId();
    console.log('ID del usuario autenticado:', idUsuario);

    if (idUsuario) {
      this.citaService.obtenerCitasPorUsuario(idUsuario).subscribe({
        next: (data) => {
          this.citas = data;
          console.log('Citas cargadas:', this.citas);
        },
        error: (err) => {
          console.error('Error al obtener citas:', err);
        },
      });
    } else {
      console.warn('Token no válido o no existe. Redirigiendo al login...');
      this.router.navigate(['/login']);
    }
  }

  irACitas() {
    this.router.navigate(['/citas']);
  }
  
  obtenerUsuarioId(): number | null {
    return this.authService.obtenerIdUsuarioDesdeToken();
  }

  eliminarCita(id: number) {
    if (confirm('¿Estás seguro de que deseas eliminar esta cita?')) {
      this.citaService.deleteCita(id).subscribe({
        next: () => {
          this.citas = this.citas.filter((c) => c.IDCita !== id);
          console.log('Cita eliminada');
        },
        error: (err) => console.error('Error al eliminar cita:', err),
      });
    }
  }
}

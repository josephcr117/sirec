import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  {
    path: 'admin',
    loadComponent: () =>
      import('./pages/admin/admin.component').then((m) => m.AdminComponent),
  },
  {
    path: 'dashboard',
    loadComponent: () =>
      import('./pages/dashboard/dashboard.component').then(
        (m) => m.DashboardComponent
      ),
  },
  {
    path: 'citas',
    loadComponent: () =>
      import('./pages/citas.component').then((m) => m.CitasComponent),
  },
  {
    path: 'editar-cita/:id',
    loadComponent: () =>
      import('./pages/editar-cita.component').then(
        (m) => m.EditarCitaComponent
      ),
  },
  {
    path: 'asignar-medico',
    loadComponent: () =>
      import('./pages/asignar-medico/asignar-medico.component').then(
        (m) => m.AsignarMedicoComponent
      ),
  },
  {
    path: 'medico',
    loadComponent: () =>
      import('./pages/medico/medico.component').then((m) => m.MedicoComponent),
  },
  {
    path: 'solicitar-cita',
    loadComponent: () =>
      import('./pages/solicitar-cita/solicitar-cita.component').then(
        (m) => m.SolicitarCitaComponent
      ),
  },
  { path: '**', redirectTo: 'login' }, 
];

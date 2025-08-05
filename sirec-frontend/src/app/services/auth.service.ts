import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'http://localhost:5147/api/Auth';

  constructor(private http: HttpClient) {}

  registrar(usuario: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, usuario);
  }

  login(datos: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, datos);
  }

  guardarToken(token: string) {
    localStorage.setItem('token', token);
  }

  obtenerToken(): string | null {
    return localStorage.getItem('token');
  }

  cerrarSesion() {
    localStorage.removeItem('token');
  }

  estaAutenticado(): boolean {
    return !!this.obtenerToken();
  }

  obtenerIDUsuario(): number | null {
    const token = this.obtenerToken();
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

  obtenerIdUsuarioDesdeToken(): number | null {
    const token = this.obtenerToken();
    if (!token) return null;

    try {
      const decoded: any = jwtDecode(token);
      return +decoded['id'] || null;
    } catch (e) {
      console.error('Error al obtener el ID del token:', e);
      return null;
    }
  }

  obtenerRol(): string | null {
    const token = this.obtenerToken();
    if (!token) return null;

    try {
      const decoded: any = jwtDecode(token);
      return decoded['role'] || null;
    } catch (e) {
      console.error('Error al decodificar el token:', e);
      return null;
    }
  }
}

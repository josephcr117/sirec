import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CitaService {
  private apiUrl = 'http://localhost:5147/api/Citas';

  constructor(private http: HttpClient) {}

  obtenerCitasPorUsuario(idUsuario: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/usuario/${idUsuario}`);
  }
  
  deleteCita(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}

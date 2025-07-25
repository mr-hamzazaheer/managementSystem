// src/app/core/http.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class HttpService {
  private readonly baseUrl = 'https://localhost:7165/api';

  constructor(private http: HttpClient) {}

  private toParams(params?: Record<string, unknown>): HttpParams {
    let httpParams = new HttpParams();
    if (params) {
      Object.entries(params).forEach(([key, val]) =>
        httpParams = httpParams.set(key, String(val))
      );
    }
    return httpParams;
  }

  get<T>(endpoint: string, params?: Record<string, unknown>): Observable<T> {
  try {
    return this.http.get<T>(`${this.baseUrl}/${endpoint}`, {
      params: this.toParams(params)
    });
  } catch (error) {
    console.error('Error in GET request:', error);
    throw error;
  }
}

  post<T>(endpoint: string, body: unknown): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}/${endpoint}`, body);
  }

  put<T>(endpoint: string, body: unknown): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/${endpoint}`, body);
  }

  delete<T>(endpoint: string, params?: Record<string, unknown>): Observable<T> {
    return this.http.delete<T>(`${this.baseUrl}/${endpoint}`, {
      params: this.toParams(params),
    });
  }
}

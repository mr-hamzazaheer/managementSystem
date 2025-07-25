// src/app/core/http.interceptor.ts
import {
  HttpInterceptorFn,
  HttpRequest,
  HttpHandlerFn,
  HttpErrorResponse,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { Router, RouterLink } from '@angular/router';

export const httpInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn) => {
  const token = localStorage.getItem('token');
  const router = inject(Router);
  // Add JSON headers and token if available
  const clonedRequest = req.clone({
    setHeaders: {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
    },
  });

  return next(clonedRequest).pipe(
    catchError((error: HttpErrorResponse) => {
      debugger
      console.error('[HTTP ERROR]', error);

      // Optional: handle specific status codes globally
      if (error.status === 500) {
          router.navigate(['/errors/500']);
      }if (error.status === 400) {
          router.navigate(['/errors/400']);
      }
      debugger
      return throwError(() => error);
    })
  );
};

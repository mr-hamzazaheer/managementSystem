import { Injectable } from '@angular/core'; 
import { HttpService } from 'src/app/shared/service/generic.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly prefix = 'auth';
  constructor(private methord: HttpService) {}
  private url(endpoint: string): string {
    return `${this.prefix}/${endpoint}`;
  }
    Login(data: any) {
        return this.methord.post(this.url('login'), data);
    }
    Register(data: any) {
        return this.methord.post(this.url('register'), data);
    }
    ForgotPassword(data: any) {
        return this.methord.post(this.url('forgot-password'), data);
    }
}

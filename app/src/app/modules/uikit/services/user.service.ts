import { Injectable } from '@angular/core'; 
import { HttpService } from 'src/app/shared/service/generic.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly prefix = 'user';
  constructor(private methord: HttpService) {}
  private url(endpoint: string): string {
    return `${this.prefix}/${endpoint}`;
  }
    GetAll(parm: any) {
        return this.methord.get(this.url('get-all'),parm);
    }
//     Register(data: any) {
//         return this.methord.post(this.url('register'), data);
//     }
//     ForgotPassword(data: any) {
//         return this.methord.post(this.url('forgot-password'), data);
//     }
 }

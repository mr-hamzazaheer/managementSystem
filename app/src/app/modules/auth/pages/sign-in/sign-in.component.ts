import { NgClass, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AngularSvgIconModule } from 'angular-svg-icon';
import { ButtonComponent } from '../../../../shared/components/button/button.component';
import { AuthService } from '../../services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css'],
  imports: [FormsModule, ReactiveFormsModule, RouterLink, AngularSvgIconModule, NgIf, ButtonComponent, NgClass],
})
export class SignInComponent implements OnInit {
  form!: FormGroup;
  submitted = false;
  passwordTextType!: boolean;
  IsLoader = false
  constructor(private readonly _formBuilder: FormBuilder, private readonly _router: Router,
    private _authService:AuthService,private _toastr: ToastrService) {}

  onClick() {
    console.log('Button clicked');
  }

  ngOnInit(): void {
    this.form = this._formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  get f() {
    return this.form.controls;
  }

  togglePasswordTextType() {
    this.passwordTextType = !this.passwordTextType;
  }

  onSubmit() {
    debugger
    this.submitted = true;
    const { email, password } = this.form.value;
    this.IsLoader = true;
    if (this.form.invalid) {
      return;
    }else{
          this._authService.Login({ email, password }).subscribe({
            next: (response) => {
                const { httpCode, data,message } = response as any;
                if (httpCode === 200) {
                  localStorage.setItem('token', data?.token);
                  this._router.navigate(['/']);
                  this._toastr.success( message);
                }else if (httpCode === 401) {
                  this._toastr.warning( message);
                }else{
                  this._toastr.error( message);
                }
                
                this.IsLoader = false;
              },
            error: (error) => {
              this._toastr.error( 'error');
              this.IsLoader= false;
              // TODO: Display user-friendly error message (e.g., incorrect credentials)
             // this.errorMessage = 'Invalid email or password.';
            }
          });
    }
  }
}

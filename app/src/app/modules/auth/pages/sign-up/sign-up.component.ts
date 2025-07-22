import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AngularSvgIconModule } from 'angular-svg-icon';
import { ButtonComponent } from 'src/app/shared/components/button/button.component';
import { AuthService } from '../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { getError } from 'src/app/shared/utils/ckassnames'; 
import { FormErrorDirective } from 'src/app/shared/directives/form-error.directive';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css'],
  imports: [FormsModule,CommonModule, RouterLink, AngularSvgIconModule, ButtonComponent,ReactiveFormsModule,FormErrorDirective],
})
export class SignUpComponent implements OnInit {
    registerForm!: FormGroup;
  passwordStrength = 0;
 getError = getError;
  constructor(private fb: FormBuilder,private readonly _router: Router, private _authService:AuthService,private _toastr: ToastrService) {}

  ngOnInit(): void {
    this.registerForm = this.fb.group(
      {
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(8)]],
        confirmPassword: ['', Validators.required],
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        contactNo: [''],
      },
      { validators: this.passwordsMatch }
    );
  }

  // get email(): AbstractControl {
  //   return this.registerForm.get('email')!;
  // }

  get password(): AbstractControl {
    return this.registerForm.get('password')!;
  }

  // get confirmPassword(): AbstractControl {
  //   return this.registerForm.get('confirmPassword')!;
  // }

  checkPasswordStrength(): void {
    const value: string = this.password.value || '';
    let strength = 0;
    if (value.length >= 8) strength++;
    if (/[A-Z]/.test(value)) strength++;
    if (/[0-9]/.test(value)) strength++;
    if (/[^A-Za-z0-9]/.test(value)) strength++;
    this.passwordStrength = strength;
  }

  passwordsMatch(control: AbstractControl): ValidationErrors | null {
    const pass = control.get('password')?.value;
    const confirmPass = control.get('confirmPassword')?.value;
    return pass === confirmPass ? null : { passwordMismatch: true };
  }

  Register(): void {
    debugger
    this.registerForm.markAllAsTouched();
    if (this.registerForm.invalid) {
      return;
    }else{
          this._authService.Register(this.registerForm.value).subscribe({
            next: (response) => {
              this._toastr.success( 'Success');

              // Store token if present
              if (response) {
               // localStorage.setItem('token', response.token);
              this._router.navigate(['auth/login']);
              } else {
              this._toastr.warning( 'warning');
              } 
            },
            error: (error) => {
              this._toastr.error( 'error');

              // TODO: Display user-friendly error message (e.g., incorrect credentials)
             // this.errorMessage = 'Invalid email or password.';
            }
          });

          console.log('Form submitted:', this.registerForm.value);

    }
  }
}

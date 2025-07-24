import { Component, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AngularSvgIconModule } from 'angular-svg-icon';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css'],
  imports: [AngularSvgIconModule, RouterOutlet],
})
export class AuthComponent implements OnInit {
  constructor(private _router:Router) {}

  ngOnInit(): void {

    if(localStorage.getItem('token')){
      // Redirect to dashboard if token exists
      this._router.navigate(['/dashboard']);
    }
  }
}

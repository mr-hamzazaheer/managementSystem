import { Component, computed, signal, Signal } from '@angular/core';
import { Router } from '@angular/router';
import { AngularSvgIconModule } from 'angular-svg-icon';
import { ButtonComponent } from 'src/app/shared/components/button/button.component';

@Component({
  selector: 'app-error404',
  imports: [AngularSvgIconModule, ButtonComponent],
  templateUrl: './error404.component.html',
  styleUrl: './error404.component.css',
})
export class Error404Component {
  write= signal('Booo');
 message = computed(() => this.write());
// message:string='Boooo!!!!';
  constructor(private router: Router) {
    debugger
  }
ngOnInit() {
  (window as any).myComp = this;
}

  goToHomePage() {
    this.router.navigate(['/']);
  }
}

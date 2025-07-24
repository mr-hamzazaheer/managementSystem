import { Directive, Input, OnInit, ElementRef, OnDestroy } from '@angular/core';
import { FormGroup, AbstractControl } from '@angular/forms';
import { Subscription } from 'rxjs';

@Directive({
  selector: '[formError]',
  standalone: true,
})
export class FormErrorDirective implements OnInit, OnDestroy {
  @Input('formError') controlName!: string;
  @Input() formGroup!: FormGroup;

  private control!: AbstractControl;
  private subscription = new Subscription();

  constructor(private el: ElementRef<HTMLElement>) {}

  ngOnInit(): void {
    debugger
    this.control = this.formGroup.get(this.controlName)!;

    // Listen to value & status changes
    this.subscription.add(
      this.control.statusChanges.subscribe(() => this.updateMessages())
    );

    // Also watch the form group (for group-level errors)
    this.subscription.add(
      this.formGroup.statusChanges.subscribe(() => this.updateMessages())
    );

    this.updateMessages();
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  private updateMessages(): void {
    const element = this.el.nativeElement;
    const messages: string[] = [];

    const control = this.control;
    const group = this.formGroup;
    if (!control) {
      element.innerHTML = 'Form control not found.';
      return;
    }   
    if (control.touched || control.dirty) {
      const errors = control.errors;
      if (errors) {
        if (errors['required']) messages.push('This field is required.');
        if (errors['email']) messages.push('Please enter a valid email address.');
        if (errors['minlength']) messages.push(`Minimum length is ${errors['minlength'].requiredLength} characters.`);
        if (errors['maxlength']) messages.push(`Maximum length is ${errors['maxlength'].requiredLength} characters.`);
        if (errors['pattern']) messages.push('Invalid format.');
        if (errors['custom']) messages.push(errors['custom']);
        if (errors['min']) messages.push(`Minimum value is ${errors['min'].min}`);
        if (errors['max']) messages.push(`Maximum value is ${errors['max'].max}`);
      }

      // Group-level error mapping (like passwordMismatch)
      if (this.controlName === 'confirmPassword' && group.errors?.['passwordMismatch']) {
        messages.push('Passwords do not match.');
      }
    }

    element.innerHTML = messages.map(msg => `<div class="text-sm text-red-500 mt-1">${msg}</div>`).join('');
  }
}

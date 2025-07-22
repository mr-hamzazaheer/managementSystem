
import { AbstractControl, FormGroup, ValidationErrors } from '@angular/forms';

export function cx(...classes: string[]) {
  return classes.filter(Boolean).join(' ');
}


export function getError(form: FormGroup, controlName: string): string | null {
  const control = form.get(controlName);
  if (!control || !(control.dirty || control.touched)) return null;

  const errors = control.errors;
  if (!errors) return null;

  return mapErrorToMessage(errors);
}

function mapErrorToMessage(errors: ValidationErrors): string {
  if (errors['required']) return 'This field is required.';
  if (errors['email']) return 'Please enter a valid email address.';
  if (errors['minlength']) return `Minimum length is ${errors['minlength'].requiredLength} characters.`;
  if (errors['maxlength']) return `Maximum length is ${errors['maxlength'].requiredLength} characters.`;
  if (errors['pattern']) return 'Invalid format.';
  if (errors['passwordMismatch']) return 'Passwords do not match.';
  if (errors['min']) return `Minimum value is ${errors['min'].min}`;
  if (errors['max']) return `Maximum value is ${errors['max'].max}`;
  if (errors['custom']) return errors['custom']; // support custom message from validators

  return 'Invalid field.';
}

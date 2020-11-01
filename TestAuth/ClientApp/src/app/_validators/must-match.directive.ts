import { Injectable } from '@angular/core';
import { FormGroup, ValidatorFn } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class MustMatchValidator {
  public match(controlName: string, controlName2: string): ValidatorFn  {
    return (control: FormGroup) => {
      return control.get(controlName).value != control.get(controlName2).value ? { match: true } : null;
    }
  };

}

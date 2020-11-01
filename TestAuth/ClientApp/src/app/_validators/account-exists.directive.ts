import { Directive, Injectable, Inject } from '@angular/core';
import { FormGroup, ValidatorFn, AbstractControl, AsyncValidator, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { UsersService } from '../_services/users.service';
import { map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AccountExistsValidator {

  constructor(
    private usersService: UsersService,
    @Inject('BASE_URL') private baseUrl: string
  ) { }

  validate(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      return this.usersService.doesUserExist(control.value).pipe(
        map(res => (res ? null : { emailExists: true })),
        catchError(() => of(null))
      );
    }
  }
}


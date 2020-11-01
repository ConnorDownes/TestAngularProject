import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../_services/authentication.service';
import { FormBuilder, FormGroup, Validators, ValidationErrors, ValidatorFn } from '@angular/forms';
import { MustMatchValidator } from '../_validators/must-match.directive';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  formSubmitted: boolean = false;

  constructor(
    private authService: AuthenticationService,
    private formBuilder: FormBuilder,
    private matchValidator: MustMatchValidator
  ) {
    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(20)]],
      passwordConfirmation: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(20)]]
    }, { validators: [this.matchValidator.match('password', 'passwordConfirmation') ]})
  }

  ngOnInit(): void {
  }

  get email() { return this.registerForm.get('email') };

  get password() { return this.registerForm.get('password') };

  get passwordConfirmation() { return this.registerForm.get('passwordConfirmation') };

  onRegisterFormSubmit(input: RegistrationModel) {
    this.formSubmitted = true;
    this.authService.register(input.email, input.password),
      error => this.formSubmitted = false;
  }
}

interface RegistrationModel {
  email: string,
  password: string,
  confirmedPassword: string
}

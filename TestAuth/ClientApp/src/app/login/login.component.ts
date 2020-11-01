import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from '../_services/authentication.service';
import { Router, ActivatedRoute } from '@angular/router';
import { finalize, catchError } from 'rxjs/operators';
import { StringHelpers } from '../_helpers/string-helpers';
import { AccountExistsValidator } from '../_validators/account-exists.directive';
import { IBasicAuth } from '../_interfaces/authentication';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  tokenForm: FormGroup;
  status: LoginStatus = "Login";
  error: string = "";
  mfaEnabled: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthenticationService,
    private router: Router,
    private route: ActivatedRoute,
    private accountExistsValidator: AccountExistsValidator,
    @Inject('BASE_URL') private baseUrl: string
  ) {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email, Validators.minLength(3)], this.accountExistsValidator.validate(), 'blur'],
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(20)]]
    });
    this.tokenForm = this.formBuilder.group({
      token: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  ngOnInit(): void {
  }

  get email() { return this.loginForm.get('email') };

  get password() { return this.loginForm.get('password') };

  get token() { return this.tokenForm.get('token') };

  onSubmit(loginFormInput: IBasicAuth) {
    if (this.loginForm.invalid) {
      return;
    }

    this.changeStatus("Loading");

    this.authService.authenticate(loginFormInput.email, loginFormInput.password)
      .subscribe(
        obj => {
          this.mfaEnabled = obj;
          if (obj) {
            this.changeStatus("2FA");
          } else {
            this.loginUser(loginFormInput);
          };
        },
        error => {
          this.changeStatus("Login");
          this.UpdatePassword('');
          this.error = error.error;
        }
    );
  }

  verifyToken(tokenFormInput) {
    this.authService.verifyTwoFactorToken({ Email: this.email.value, TwoFactorCode: tokenFormInput.token }).subscribe(
      obj => {
        if (obj) {
          this.loginUser(this.loginForm.value);
        }
      },
      error => {
        this.error = error.error;
      }
    )
  }

  private loginUser(loginInfo: IBasicAuth) {
    if (this.loginForm.invalid) {
      return;
    }

    this.changeStatus("Loading");

    this.authService.login(loginInfo.email, loginInfo.password)
      .subscribe(res => { },
        error => {
          this.changeStatus("Login");
          this.error = error;
        },
        () => {
          console.log("Password Success!");
          
          let returnUrl = '/';
          if (this.route.snapshot.queryParamMap.has('returnUrl')) {
            returnUrl = this.route.snapshot.queryParamMap.get('returnUrl');
          } 

          if (!this.mfaEnabled) {
            this.router.navigateByUrl("/security?" + returnUrl);
          } else {
            this.router.navigateByUrl(returnUrl);
          }
        }
    );
  }

  onReturnClick() {
    this.changeStatus("Login");
    this.UpdatePassword('');
    this.UpdateEmail('');
    this.UpdateToken('');
    this.loginForm.markAsUntouched();
    this.loginForm.markAsPristine();
    this.tokenForm.markAsUntouched();
    this.tokenForm.markAsPristine();
  }

  private UpdatePassword(newValue: string) {
    this.loginForm.patchValue({
      password: newValue
    })
  }

  private UpdateEmail(newValue: string) {
    this.loginForm.patchValue({
      email: newValue
    })
  }

  private UpdateToken(newValue: string) {
    this.tokenForm.patchValue({
      token: newValue
    })
  }

  private changeStatus(status: LoginStatus) {
    this.status = status;
    this.error = "";
  }

}

export type LoginStatus = "Login" | "2FA" | "Loading";

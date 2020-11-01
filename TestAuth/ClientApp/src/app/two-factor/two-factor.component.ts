import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../_services/authentication.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'two-factor-mfa',
  templateUrl: './two-factor.component.html',
  styleUrls: ['./two-factor.component.css']
})
export class TwoFactorComponent implements OnInit {
  twoFactorForm: FormGroup;
  isMfaEnabled: boolean;
  qrUri: string = null;
  key: string = null;
  recoveryCodes: string[] = [];
  tokenError: string = null;

  constructor(
    private authService: AuthenticationService,
    private formBuilder: FormBuilder,
  ) {
    this.twoFactorForm = this.formBuilder.group({
      TwoFactorCode: ['', [Validators.required]]
    });

    this.init();
  }

  get twoFactor() { return this.twoFactorForm.get('TwoFactorCode') };

  ngOnInit(): void {
    
  }

  verifyToken(tokenForm) {
    console.log(tokenForm.TwoFactorCode);
    this.authService.setupTwoFactor(tokenForm.TwoFactorCode)
      .subscribe(obj => {
        console.log(obj);
        this.getRecoveryCodes();
        this.init();
      },
        error => {
          this.tokenError = error;
        })
      
  }

  private init() {
    this.authService.getTwoFactorDetails().subscribe(obj => {
      this.isMfaEnabled = obj.isMfaEnabled;
      console.log("MFA Enabled: " + obj.isMfaEnabled);
      if (!obj.isMfaEnabled) {
        this.qrUri = obj.uri;
        this.key = obj.key;
        this.recoveryCodes = [];
      }
    });
  }

  onGenerateCodesClick() {
    this.getRecoveryCodes();
  }

  private getRecoveryCodes() {
    this.authService.generateTwoFactorRecoveryCodes().subscribe(codes => {
      this.recoveryCodes = codes;
    })
  }

  disableMfa() {
    console.log("Disabling 2FA");
    this.authService.disableTwoFactor().subscribe(obj => {
      console.log(obj);
      this.init();
    })
  }

}

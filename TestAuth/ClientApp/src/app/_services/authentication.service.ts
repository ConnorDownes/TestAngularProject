import { Injectable, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { tap, shareReplay, finalize, catchError, map } from 'rxjs/operators';
import { ITokenWithExpiry, ITokenWithExpiredFlag, IMfaDetails, IVerifyMfaAuth } from '../_interfaces/authentication'
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})

export class AuthenticationService implements OnInit {
  private userLoggedIn = new BehaviorSubject<boolean>(false);

  constructor(
    private client: HttpClient,
    private router: Router,
    @Inject('BASE_URL') private baseUrl: string
  ) {
    if (!this.isTokenExpired()) {
      this.userLoggedIn.next(true);
    } else {
      this.userLoggedIn.next(false);
    }
  }

  ngOnInit() {
    
  }

  errorHandler(error: HttpErrorResponse) {
    return throwError(error.message || "server error.");
  }

  getTwoFactorStatus() {
    return this.client.get<boolean>(this.baseUrl + 'api/Account/auth/status').pipe(
      map(obj => {
        return obj;
      })
    )
  }

  authenticate(Email: string, Password: string): Observable<boolean> {
    return this.client.post<boolean>(this.baseUrl + 'api/Account/authenticate',
      { Email: Email, Password: Password }
    ).pipe(
      obj => {
      return obj;
    });
  }

  setupTwoFactor(token: string): Observable<string> {
    return this.client.post(this.baseUrl + 'api/Account/auth/enable',
      { TwoFactorCode: token },
      { responseType: 'text' }
    ).pipe(
        map(obj => {
          console.log("Enable Object: " + obj);
          return obj;
        })
    );
  }

  disableTwoFactor(): Observable<string> {
    return this.client.post(this.baseUrl + 'api/Account/auth/disable', null, { responseType: 'text' })
      .pipe(
        map(obj => {
          console.log("Disable Object: " + obj);
          return obj;
        })
      );
  }

  getTwoFactorDetails(): Observable<IMfaDetails> {
    return this.client.get<IMfaDetails>(this.baseUrl + 'api/Account/auth/verify')
      .pipe(obj => {
        return obj;
      })
  }

  generateTwoFactorRecoveryCodes(): Observable<string[]> {
    return this.client.get<string[]>(this.baseUrl + 'api/Account/auth/recovery')
      .pipe(codes => {
        return codes;
      })
  }

  verifyTwoFactorToken(token: IVerifyMfaAuth): Observable<boolean> {
    console.log(token.Email, token.TwoFactorCode);
    return this.client.post<boolean>(this.baseUrl + 'api/Account/auth/verify',
      { Email: token.Email, TwoFactorCode: token.TwoFactorCode }
    ).pipe(
      map(obj => {
        return obj;
        }
      )
    );
  }

  register(Email: string, Password: string) {
    this.client.post(this.baseUrl + 'api/Account/register',
      { Email: Email, Password: Password },
      { responseType: 'text' }
    ).subscribe(res => console.log(res)),
      error => console.error(error),
      () => this.router.navigate(["/"]);
  }

  login(Email: string, Password: string): Observable<ITokenWithExpiry> {
    return this.client.post<ITokenWithExpiry>(this.baseUrl + 'api/Account/login', { Email: Email, Password: Password })
      .pipe(tap(
        res => {
          this.setSession(res);
          this.userLoggedIn.next(true);
        }
      ),
        shareReplay(),
      );
  }

  private setSession(authResult: ITokenWithExpiry) {
    const dateNow = new Date();
    const expiresAt = dateNow.setSeconds(dateNow.getSeconds() + authResult.expiresIn);

    console.warn("Token generated:", authResult.token);
    console.warn("Expires at:", expiresAt);

    localStorage.setItem("id_token", authResult.token);
    localStorage.setItem("expires_at", JSON.stringify(expiresAt.valueOf()));
  }

  getExpiryToken() : ITokenWithExpiredFlag {
    return { token: localStorage.getItem('id_token'), isExpired: this.isTokenExpired() };
  }

  logout() {
    localStorage.removeItem("id_token");
    localStorage.removeItem("expires_at");
    this.userLoggedIn.next(false);
    this.router.navigate(["/login"]);
  }

  get isUserLoggedIn() {
    return this.userLoggedIn.asObservable(); 
  }

  private isTokenExpired() {
    let current = new Date().getTime(); // current date
    let expiry = this.getExpiration(); // token expiry date
    return current > expiry;
  }

  getExpiration() {
    const expiration = localStorage.getItem("expires_at");
    const expiresAt = JSON.parse(expiration);
    return expiresAt;
  }
}


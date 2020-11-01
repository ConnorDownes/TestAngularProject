import { Injectable, Inject } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../_services/authentication.service';
import { take, map, switchMap } from 'rxjs/operators';
import { ITokenWithExpiredFlag } from '../_interfaces/authentication';
import { StringHelpers } from '../_helpers/string-helpers';

@Injectable()
export class AuthorisationInterceptor implements HttpInterceptor {

  constructor(
    private authService: AuthenticationService,
    private str: StringHelpers,
    @Inject('BASE_URL') private baseUrl: string
  ) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return this.authService.isUserLoggedIn.pipe(
      take(1),
      switchMap((isloggedin: boolean) => {
        let newRequest = request;

        if (isloggedin && request.url.indexOf(this.baseUrl) > -1) {
          let tokencontainer: ITokenWithExpiredFlag = this.authService.getExpiryToken();
          let token = tokencontainer.token;

          if (!this.str.isEmpty(token))
            newRequest = request.clone({ setHeaders: { Authorization: "Bearer " + token } });
        }

        return next.handle(newRequest);
        })
    )
  }
}

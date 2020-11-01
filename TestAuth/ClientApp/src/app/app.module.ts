import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { QRCodeModule } from 'angularx-qrcode';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { StringHelpers } from './_helpers/string-helpers';
import { LoaderComponent } from './loader/loader.component';
import { AuthenticatedRouteGuard, LoggedInRouteGuard } from './_authGuards/authenticated-route.guard';
import { AuthenticationService } from './_services/authentication.service';
import { ImageWithTextComponent } from './image-with-text/image-with-text.component';
import { AuthorisationInterceptor } from './_interceptors/authorisation.interceptor';
import { TwoFactorComponent } from './two-factor/two-factor.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    LoginComponent,
    RegisterComponent,
    LoaderComponent,
    ImageWithTextComponent,
    TwoFactorComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    NgbModule,
    ReactiveFormsModule,
    QRCodeModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [AuthenticatedRouteGuard] },
      { path: 'counter', component: CounterComponent, canActivate: [AuthenticatedRouteGuard] },
      { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthenticatedRouteGuard] },
      { path: 'login', component: LoginComponent, canActivate: [LoggedInRouteGuard] },
      { path: 'register', component: RegisterComponent, canActivate: [LoggedInRouteGuard] },
      { path: 'security', component: TwoFactorComponent, canActivate: [AuthenticatedRouteGuard] }
    ]),
    NgbModule
  ],
  providers: [StringHelpers, AuthenticatedRouteGuard, AuthenticationService, LoggedInRouteGuard, { provide: HTTP_INTERCEPTORS, useClass: AuthorisationInterceptor, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule {}

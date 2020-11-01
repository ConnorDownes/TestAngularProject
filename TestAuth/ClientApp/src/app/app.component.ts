import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from './_services/authentication.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  title = 'app';
  isLoggedIn$: Observable<boolean>;

  constructor(
    private authService: AuthenticationService
  ) { }

  ngOnInit(): void {
    this.isLoggedIn$ = this.authService.isUserLoggedIn;
  }
}

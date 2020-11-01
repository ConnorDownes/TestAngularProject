import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { map, take } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(
    private client: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
  ) { }

  doesUserExist(email: string): Observable<boolean> {
    return this.client.get<boolean>(this.baseUrl + 'api/Account/exists?EmailAddress=' + email)
      .pipe(take(1))
  }
}

import { TestBed } from '@angular/core/testing';

import { AuthenticatedRouteGuard } from './authenticated-route.guard';

describe('AuthenticatedRouteGuard', () => {
  let guard: AuthenticatedRouteGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(AuthenticatedRouteGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});

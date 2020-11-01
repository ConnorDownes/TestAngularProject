import { TestBed } from '@angular/core/testing';

import { MustMatchValidator } from '../must-match.directive';

describe('MustMatchService', () => {
  let service: MustMatchValidator;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MustMatchValidator);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

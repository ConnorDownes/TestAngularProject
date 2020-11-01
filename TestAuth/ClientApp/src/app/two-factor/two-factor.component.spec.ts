import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EnableMFAComponent } from './enable-mfa.component';

describe('EnableMFAComponent', () => {
  let component: EnableMFAComponent;
  let fixture: ComponentFixture<EnableMFAComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EnableMFAComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EnableMFAComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

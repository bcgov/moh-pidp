import { TestBed } from '@angular/core/testing';

import { LoginPage } from './login.page';

describe('LoginPage', () => {
  let component: LoginPage;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LoginPage],
    });
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

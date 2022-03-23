import { TestBed } from '@angular/core/testing';

import { SelfDeclarationPage } from './self-declaration.page';

describe('SelfDeclarationPage', () => {
  let component: SelfDeclarationPage;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SelfDeclarationPage],
    });

    component = TestBed.inject(SelfDeclarationPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

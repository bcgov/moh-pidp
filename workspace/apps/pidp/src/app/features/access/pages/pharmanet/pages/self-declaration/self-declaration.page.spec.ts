import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfDeclarationPage } from './self-declaration.page';

describe('SelfDeclarationPage', () => {
  let component: SelfDeclarationPage;
  let fixture: ComponentFixture<SelfDeclarationPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SelfDeclarationPage],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfDeclarationPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

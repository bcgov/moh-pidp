import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LinkAccountErrorPage } from './link-account-error.page';

describe('LinkAccountErrorPage', () => {
  let component: LinkAccountErrorPage;
  let fixture: ComponentFixture<LinkAccountErrorPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LinkAccountErrorPage],
    }).compileComponents();

    fixture = TestBed.createComponent(LinkAccountErrorPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

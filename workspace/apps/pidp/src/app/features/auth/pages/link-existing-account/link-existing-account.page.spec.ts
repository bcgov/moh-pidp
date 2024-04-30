import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LinkExistingAccountPage } from './link-existing-account.page';

describe('LinkExistingAccountPage', () => {
  let component: LinkExistingAccountPage;
  let fixture: ComponentFixture<LinkExistingAccountPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LinkExistingAccountPage],
    }).compileComponents();

    fixture = TestBed.createComponent(LinkExistingAccountPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

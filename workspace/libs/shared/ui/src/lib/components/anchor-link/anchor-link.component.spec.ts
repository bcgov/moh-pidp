import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnchorLinkComponent } from './anchor-link.component';

describe('AnchorLinkComponent', () => {
  let component: AnchorLinkComponent;
  let fixture: ComponentFixture<AnchorLinkComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AnchorLinkComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AnchorLinkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

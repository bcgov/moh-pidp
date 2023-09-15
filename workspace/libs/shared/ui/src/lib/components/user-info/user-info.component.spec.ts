import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DefaultPipe, FullnamePipe } from '../../pipes';
import { KeyValueInfoComponent } from '../key-value-info/key-value-info.component';
import { UserInfoComponent } from './user-info.component';

describe('UserInfoComponent', () => {
  let component: UserInfoComponent;
  let fixture: ComponentFixture<UserInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        UserInfoComponent,
        DefaultPipe,
        FullnamePipe,
        KeyValueInfoComponent,
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

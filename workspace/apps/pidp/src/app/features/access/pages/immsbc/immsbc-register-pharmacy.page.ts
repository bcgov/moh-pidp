import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-immsbc-register-pharmacy',
  standalone: true,
  template: `<div>Redirecting to registration form...</div>`,
})
export class ImmsbcRegisterPharmacyPage implements OnInit {
  public ngOnInit(): void {
    window.location.href = '/api/pharmacies/register-pharmacy';
  }
}

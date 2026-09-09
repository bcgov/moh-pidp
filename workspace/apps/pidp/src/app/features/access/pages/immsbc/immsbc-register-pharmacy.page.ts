import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-immsbc-register-pharmacy',
  standalone: true,
  template: `<div style='margin-top: 100px; text-align: center;'>Redirecting to registration form...</div>`,
})
export class ImmsbcRegisterPharmacyPage implements OnInit {
  public ngOnInit(): void {
    window.location.href = '/api/pharmacies/register-pharmacy';
  }
}

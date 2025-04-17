import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-external-accounts',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './external-accounts.page.html',
  styleUrl: './external-accounts.page.scss',
})
export class ExternalAccountsPage {}

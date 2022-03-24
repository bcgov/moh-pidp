import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-user-access-agreement',
  templateUrl: './user-access-agreement.page.html',
  styleUrls: ['./user-access-agreement.page.scss'],
})
export class UserAccessAgreementPage {
  public title: string;

  public constructor(private route: ActivatedRoute) {
    this.title = this.route.snapshot.data.title;
  }

  public onSubmit(): void {
    console.log('ON_SUBMIT');
  }
}

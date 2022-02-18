import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-compliance-training',
  templateUrl: './compliance-training.page.html',
  styleUrls: ['./compliance-training.page.scss'],
})
export class ComplianceTrainingPage implements OnInit {
  public title: string;

  public constructor(private route: ActivatedRoute) {
    this.title = this.route.snapshot.data.title;
  }

  public onSubmit(): void {}

  public ngOnInit(): void {}
}

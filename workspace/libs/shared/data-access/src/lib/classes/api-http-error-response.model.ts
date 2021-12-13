import { HttpHeaders } from '@angular/common/http';

export class ApiHttpErrorResponse {
  constructor(
    public readonly status: number,
    public readonly headers: HttpHeaders,
    public readonly errors: any | null,
    public readonly message: string
  ) {}
}

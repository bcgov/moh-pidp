import { HttpHeaders } from '@angular/common/http';

export class ApiHttpResponse<T> {
  public constructor(
    public readonly status: number,
    public readonly headers: HttpHeaders,
    public readonly body: T | null
  ) {}
}

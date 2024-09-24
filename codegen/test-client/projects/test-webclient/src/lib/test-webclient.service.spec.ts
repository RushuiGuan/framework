import { TestBed } from '@angular/core/testing';

import { TestWebclientService } from './test-webclient.service';

describe('TestWebclientService', () => {
  let service: TestWebclientService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TestWebclientService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

export interface ITokenWithExpiry {
  token: string;
  expiresIn: number;
}

export interface ITokenWithExpiredFlag {
  token: string;
  isExpired: boolean;
}

export interface IMfaDetails {
  isMfaEnabled: boolean;
  key: string;
  uri: string;
}

export interface IVerifyMfaAuth {
  Email: string;
  TwoFactorCode: string;
}

export interface IBasicAuth {
  email: string;
  password: string;
}

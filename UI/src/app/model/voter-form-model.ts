export class VotingForm 
{
    nomId: string;
    NomineePayrollId: number;
    Impact: string;
    BeASpark: string;
    VoterPayrollId: number;
    Period: string;
    Shortlisted?: boolean;
    ManagersRationale?: string;
    WinnerRationale?: string;
    ManagersVotes?: number;
    AdminRationale?: string;
    Winner?: boolean;
    DateCreated: Date;
    DateLastModified: Date;
    ModifiedBy?: string;
    eventId: number;
    voucherId?: number;
}
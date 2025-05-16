namespace LR4.Interfaces
{
    public interface IAdminService
    {
        string[] GetPendingRegistrations();
        void ApproveRegistration(string userData);
        void RejectRegistration(string userData);
    }
}

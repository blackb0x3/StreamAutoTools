namespace StreamInstruments.DataObjects;

public class Rule : EntityBase
{
    public Rule()
    {
        Actions = new List<RuleAction>();
    }

    /// <summary>
    /// The name of the rule. Set by the streamer so they can recognise it for later.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The Twitch stream event this rule is activated on.
    /// </summary>
    public RuleEvent Event { get; set; }

    /// <summary>
    /// Determines if the rule will be utilised.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Determines if the rule is marked as deleted. Allows for easy undo in the future.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// The Twitch username of the viewer that this rule activates for.
    /// Will apply to any viewer if this is left null.
    /// </summary>
    public string AssociatedViewerName { get; set; }

    /// <summary>
    /// The Twitch ID of the reward that this rule activates for.
    /// Will apply to any reward if this is left null.
    /// </summary>
    public string AssociatedRewardId { get; set; }

    /// <summary>
    /// The list of actions performed when the rule gets activated.
    /// </summary>
    public virtual ICollection<RuleAction> Actions { get; set; }
}
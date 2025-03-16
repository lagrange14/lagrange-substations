using Content.Client.UserInterface.Controls;
using JetBrains.Annotations;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Robust.Client.UserInterface;
using Content.Shared.Storage;
using System.Numerics;
using Robust.Shared.GameObjects;

namespace Content.Client._DV.Augments;

[GenerateTypedNameReferences]
public sealed partial class AugmentToolPanelMenu : RadialMenu
{
    [Dependency] private readonly EntityManager _entManager = default!;

    public event Action<EntityUid?>? SendAugmentToolPanelSystemMessageAction;

    private EntityUid _owner;

    public AugmentToolPanelMenu()
    {
        IoCManager.InjectDependencies(this);
        RobustXamlLoader.Load(this);
    }

    public void SetEntity(EntityUid uid)
    {
        _owner = uid;
        Refresh();
    }

    public void Refresh()
    {
        if (!_entManager.TryGetComponent<StorageComponent>(_owner, out var storage))
            return;

        foreach (var (entity, _) in storage.StoredItems)
        {
            var button = new RadialMenuTextureButtonWithSector()
            {
                SetSize = new Vector2(64f, 64f),
            };

            button.AddChild(new SpriteView(entity, _entManager)
                {
                    Scale = new Vector2(3f, 3f),
                });
            Main.AddChild(button);

            button.OnButtonUp += _ =>
            {
                SendAugmentToolPanelSystemMessageAction?.Invoke(entity);
                Close();
            };
        }

        var nilButton = new RadialMenuTextureButtonWithSector()
        {
            SetSize = new Vector2(64f, 64f),
        };

        Main.AddChild(nilButton);

        nilButton.OnButtonUp += _ =>
        {
            SendAugmentToolPanelSystemMessageAction?.Invoke(null);
            Close();
        };
    }
}
